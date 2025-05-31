import ProjectDetails from '@/components/projects/ProjectDetails';

interface ProjectPageProps {
    params: {
        id: string;
    };
}

export default function ProjectPage({ params }: ProjectPageProps) {
    return <ProjectDetails projectId={params.id} />;
}